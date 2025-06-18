import { Component, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { IonButton } from '@ionic/angular';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-workout-table',
  templateUrl: './workout-table.component.html',
  styleUrls: ['./workout-table.component.scss'],
})
export class WorkoutTableComponent implements AfterViewInit {
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

  public events: { [key: string]: string[] } = {
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

  ngAfterViewInit(): void {
    this.setCurrentDay();

    this.intervalId = setInterval(() => this.setCurrentDay(), 5000);
  }

  onButtonClick(): void {
    const defaultDay = this.days[0];
    this.events[defaultDay].push(`Event ${Date.now()}`);
  }

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }
  }

  setCurrentDay(): void {
    const date = new Date();
    const currentDay = this.days[date.getDay()];

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
    this.requestedView = localStorage.getItem('requestedView');
    const tableHeaders = Array.from(
      this.workoutTable.nativeElement.querySelectorAll<HTMLDivElement>('.workout-table__header')
    );

    tableHeaders.forEach((header) =>
      header.parentElement.classList.remove('workout-table__column--active')
    );

    switch (this.requestedView) {
      case 'week':
        tableHeaders.forEach((header) =>
          header.parentElement.classList.add('workout-table__column--active')
        );
        break;
      case 'three-day':
        let startIndex = this.currentDayIndex === 0 ? 4 : this.currentDayIndex - 2;
        startIndex = this.currentDayIndex === 1 ? 0 : startIndex;
        for (let i = 0; i < 3; i++) {
          const index = startIndex + i;
          if (tableHeaders[index]) {
            tableHeaders[index].parentElement.classList.add('workout-table__column--active');
          }
        }
        break;
      case 'day':
        if (this.currentDay) {
          this.currentDay.forEach((header) =>
            header.parentElement.classList.add('workout-table__column--active')
          );
        }
        break;
      default:
        tableHeaders.forEach((header) =>
          header.parentElement.classList.add('workout-table__column--active')
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
}
