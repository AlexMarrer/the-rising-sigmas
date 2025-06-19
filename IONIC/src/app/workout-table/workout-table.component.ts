import { Component, AfterViewInit, ViewChild, ElementRef, OnDestroy, OnInit } from '@angular/core';
import { IonButton, ModalController } from '@ionic/angular';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { ExerciseService, Exercise, UpdateExerciseRequest } from '../services/exercise.service';
import { AddExerciseModalComponent } from '../add-exercise-modal/add-exercise-modal.component';

@Component({
  selector: 'app-workout-table',
  templateUrl: './workout-table.component.html',
  styleUrls: ['./workout-table.component.scss'],
})
export class WorkoutTableComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('addButton', { static: false }) button: IonButton;
  @ViewChild('workoutTable', { static: false }) workoutTable: ElementRef<HTMLDivElement>;

  private currentDay: HTMLDivElement[] = [];
  private currentDayIndex: number;
  public days: string[] = [
    'monday',
    'tuesday',
    'wednesday',
    'thursday',
    'friday',
    'saturday',
    'sunday',
  ];
  public events: { [key: string]: Exercise[] } = {
    monday: [],
    tuesday: [],
    wednesday: [],
    thursday: [],
    friday: [],
    saturday: [],
    sunday: [],
  };

  private intervalId: any;
  public requestedView: string;

  // Exercise-related properties
  exercises: Exercise[] = [];

  constructor(private modalController: ModalController, private exerciseService: ExerciseService) {}
  ngOnInit() {
    this.loadExercises();
  }

  ngAfterViewInit(): void {
    this.setCurrentDay();
    this.intervalId = setInterval(() => this.setCurrentDay(), 5000);
  }

  loadExercises() {
    try {
      this.exerciseService.getAllExercises().subscribe({
        next: (exercises) => {
          this.exercises = exercises;
          this.organizeExercisesByDay();
        },
        error: (error) => {
          console.error('Error loading exercises:', error);
        },
      });
    } catch (error) {
      console.error('Error loading exercises:', error);
    }
  }
  organizeExercisesByDay() {
    // Reset events for each day
    this.days.forEach((day) => (this.events[day] = []));

    // Organize exercises by day (store actual Exercise objects)
    this.exercises.forEach((exercise) => {
      let dayName = this.getDayNameFromBitValue(exercise.day);
      if (dayName && this.events[dayName]) {
        this.events[dayName].push(exercise);
      }
    });
  }
  getDayNameFromBitValue(dayIndex: number): string {
    // Updated to match the new day mapping (0=Sunday, 1=Monday, etc.)
    const dayMap: { [key: number]: string } = {
      0: 'sunday',
      1: 'monday',
      2: 'tuesday',
      3: 'wednesday',
      4: 'thursday',
      5: 'friday',
      6: 'saturday',
    };
    return dayMap[dayIndex] || '';
  }

  getExerciseDisplayText(exercise: Exercise): string {
    return `${exercise.exerciseTemplate?.name || 'Unknown'} - ${exercise.sets}x${
      exercise.reps
    } @RPE ${exercise.rpe}`;
  }

  async onButtonClick(): Promise<void> {
    await this.openAddExerciseModal();
  }
  async openAddExerciseModal() {
    const modal = await this.modalController.create({
      component: AddExerciseModalComponent,
      cssClass: 'add-exercise-modal',
    });

    modal.onDidDismiss().then((result) => {
      this.loadExercises();
    });

    return await modal.present();
  }

  drop(event: CdkDragDrop<Exercise[]>) {
    console.log('Drop event triggered!', event);

    if (event.previousContainer === event.container) {
      // Reordering within the same day
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
      console.log(
        'Exercise reordered within same day from index',
        event.previousIndex,
        'to',
        event.currentIndex
      );
    } else {
      // Moving between different days
      const movedExercise = event.previousContainer.data[event.previousIndex];

      // Get the new day from the container ID
      const newDayName = event.container.id;
      const newDayIndex = this.getDayIndexFromName(newDayName);

      console.log(
        `Moving exercise from day ${movedExercise.day} to day ${newDayIndex} (${newDayName})`
      );

      // Transfer the exercise to the new day
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );

      // Update the exercise in the backend
      this.updateExerciseDay(movedExercise, newDayIndex);
    }
  }
  private getDayIndexFromName(dayName: string): number {
    const dayMap: { [key: string]: number } = {
      sunday: 0,
      monday: 1,
      tuesday: 2,
      wednesday: 3,
      thursday: 4,
      friday: 5,
      saturday: 6,
    };
    return dayMap[dayName] || 1; // Default to Monday
  }
  private updateExerciseDay(exercise: Exercise, newDay: number) {
    // Create update request with the new day
    const updateRequest: UpdateExerciseRequest = {
      reps: exercise.reps,
      sets: exercise.sets,
      rpe: exercise.rpe,
      day: newDay,
      notes: exercise.notes,
      exerciseTemplateId: exercise.exerciseTemplateId,
    };

    console.log('Updating exercise day:', updateRequest);

    // Call backend to update exercise
    this.exerciseService.updateExercise(exercise.id, updateRequest).subscribe({
      next: (updatedExercise) => {
        console.log('Exercise updated successfully:', updatedExercise);
        // Update the local exercise object
        exercise.day = newDay;
      },
      error: (error) => {
        console.error('Error updating exercise:', error);
        // Revert the change on error
        this.loadExercises();
      },
    });
  }

  setCurrentDay(): void {
    const date = new Date();
    // Fix Sunday mapping: JavaScript getDay() returns 0 for Sunday, 1 for Monday, etc.
    // We want Monday=0 in our days array, so we adjust:
    const dayIndex = date.getDay() === 0 ? 6 : date.getDay() - 1;
    const currentDay = this.days[dayIndex];

    if (this.currentDayIndex === date.getDay()) {
      return;
    }

    this.currentDayIndex = date.getDay();
    this.changeCurrentDay(currentDay);
  }

  changeView(event: any): void {
    const requestedView = event.detail.value;
    if (!this.workoutTable || !this.workoutTable.nativeElement) {
      return;
    }

    // add or set requestedView to localstorage
    localStorage.setItem('requestedView', requestedView);
    this.changeTableView();
  }
  changeTableView() {
    this.requestedView = localStorage.getItem('requestedView') || 'week';
    const tableHeaders = Array.from(
      this.workoutTable.nativeElement.querySelectorAll<HTMLDivElement>('.workout-table__header')
    );

    tableHeaders.forEach((header) =>
      header.parentElement?.classList.remove('workout-table__column--active')
    );

    switch (this.requestedView) {
      case 'week':
        tableHeaders.forEach((header) =>
          header.parentElement?.classList.add('workout-table__column--active')
        );
        break;
      case 'three-day':
        let startIndex = this.currentDayIndex === 0 ? 4 : this.currentDayIndex - 2;
        startIndex = this.currentDayIndex === 1 ? 0 : startIndex;
        for (let i = 0; i < 3; i++) {
          const index = startIndex + i;
          if (tableHeaders[index]) {
            tableHeaders[index].parentElement?.classList.add('workout-table__column--active');
          }
        }
        break;
      case 'day':
        if (this.currentDay) {
          this.currentDay.forEach((header) =>
            header.parentElement?.classList.add('workout-table__column--active')
          );
        }
        break;
      default:
        tableHeaders.forEach((header) =>
          header.parentElement?.classList.add('workout-table__column--active')
        );
        break;
    }
  }

  changeCurrentDay(currentDay: string): void {
    if (!this.workoutTable || !this.workoutTable.nativeElement) {
      return;
    }

    const tableHeaders = Array.from(
      this.workoutTable.nativeElement.querySelectorAll<HTMLDivElement>('.workout-table__header')
    );
    const lastDays = Array.from(
      this.workoutTable.nativeElement.querySelectorAll<HTMLDivElement>(
        '.workout-table__header--current-day'
      )
    );

    lastDays.forEach((lastDay) => lastDay.classList.remove('workout-table__header--current-day'));

    const filteredHeader = tableHeaders.filter((header) => header.dataset['day'] === currentDay);

    filteredHeader.forEach((header) => {
      header.classList.add('workout-table__header--current-day');
    });

    this.currentDay = filteredHeader;
  }

  // Legacy methods for compatibility
  addRandomExercise() {
    this.openAddExerciseModal();
  }

  isCurrentDay(day: string): boolean {
    const date = new Date();
    const dayIndex = date.getDay() === 0 ? 6 : date.getDay() - 1;
    const currentDay = this.days[dayIndex];
    return day === currentDay;
  }

  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }
}
