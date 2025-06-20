import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import {
  ExerciseService,
  ExerciseTemplate,
  MuscleGroup,
  CreateExerciseRequest,
  CreateExerciseTemplateRequest,
} from '../services/exercise.service';

@Component({
  selector: 'app-add-exercise-modal',
  templateUrl: './add-exercise-modal.component.html',
  styleUrls: ['./add-exercise-modal.component.scss'],
})
export class AddExerciseModalComponent implements OnInit {
  exerciseTemplates: ExerciseTemplate[] = [];
  muscleGroups: MuscleGroup[] = [];
  selectedMuscleGroupId: string = '';
  filteredExerciseTemplates: ExerciseTemplate[] = [];

  exerciseForm = {
    exerciseTemplateId: '',
    sets: 3,
    reps: 10,
    rpe: 7.0,
    day: 1, // Monday
    notes: '',
    selectedDays: [] as number[], // Für mehrere Tage
  };
  // Template creation form
  templateForm = {
    name: '',
    muscleGroupId: '',
    muscleGroupName: '', // Für neue Muskelgruppe
    createNewMuscleGroup: false, // Toggle für neue Muskelgruppe
  };

  showCreateTemplate = false;
  days = [
    { value: 1, label: 'Montag', selected: false },
    { value: 2, label: 'Dienstag', selected: false },
    { value: 3, label: 'Mittwoch', selected: false },
    { value: 4, label: 'Donnerstag', selected: false },
    { value: 5, label: 'Freitag', selected: false },
    { value: 6, label: 'Samstag', selected: false },
    { value: 7, label: 'Sonntag', selected: false },
  ];

  constructor(private modalController: ModalController, private exerciseService: ExerciseService) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.exerciseService.getAllExerciseTemplates().subscribe({
      next: (templates) => {
        this.exerciseTemplates = templates;
        this.filteredExerciseTemplates = templates;
      },
      error: (error) => console.error('Error loading exercise templates:', error),
    });

    this.exerciseService.getAllMuscleGroups().subscribe({
      next: (muscleGroups) => {
        this.muscleGroups = muscleGroups;
      },
      error: (error) => console.error('Error loading muscle groups:', error),
    });
  }
  onMuscleGroupChange() {
    if (this.selectedMuscleGroupId) {
      this.filteredExerciseTemplates = this.exerciseTemplates.filter(
        (template) => template.muscleGroupId === this.selectedMuscleGroupId
      );
    } else {
      this.filteredExerciseTemplates = this.exerciseTemplates;
    }
    this.exerciseForm.exerciseTemplateId = '';
  }

  onDayToggle(day: any) {
    day.selected = !day.selected;
    this.updateSelectedDays();
  }

  updateSelectedDays() {
    this.exerciseForm.selectedDays = this.days
      .filter((day) => day.selected)
      .map((day) => day.value);
  }

  toggleCreateTemplate() {
    this.showCreateTemplate = !this.showCreateTemplate;
    if (this.showCreateTemplate) {
      this.templateForm.muscleGroupId = this.selectedMuscleGroupId;
    }
  }
  async createNewTemplate() {
    if (!this.templateForm.name) {
      console.error('Name ist erforderlich');
      return;
    }

    // Prüfe ob neue Muskelgruppe oder bestehende verwendet werden soll
    if (this.templateForm.createNewMuscleGroup) {
      if (!this.templateForm.muscleGroupName) {
        console.error('Name der neuen Muskelgruppe ist erforderlich');
        return;
      }
    } else {
      if (!this.templateForm.muscleGroupId) {
        console.error('Muskelgruppe ist erforderlich');
        return;
      }
    }

    const request: CreateExerciseTemplateRequest = {
      name: this.templateForm.name,
      muscleGroupId: this.templateForm.createNewMuscleGroup
        ? undefined
        : this.templateForm.muscleGroupId,
      muscleGroupName: this.templateForm.createNewMuscleGroup
        ? this.templateForm.muscleGroupName
        : undefined,
    };

    this.exerciseService.createExerciseTemplate(request).subscribe({
      next: (template) => {
        console.log('Template created:', template);
        // Reload templates and muscle groups
        this.loadData();
        // Select the new template
        this.exerciseForm.exerciseTemplateId = template.id;
        // Hide create form
        this.showCreateTemplate = false;
        // Reset form
        this.templateForm = {
          name: '',
          muscleGroupId: '',
          muscleGroupName: '',
          createNewMuscleGroup: false,
        };
      },
      error: (error) => {
        console.error('Error creating template:', error);
      },
    });
  }

  dismiss() {
    this.modalController.dismiss();
  }
  async createExercise() {
    if (!this.exerciseForm.exerciseTemplateId) {
      console.error('Please select an exercise template');
      return;
    }

    // Use selected days if any, otherwise use the single day
    const daysToCreate =
      this.exerciseForm.selectedDays.length > 0
        ? this.exerciseForm.selectedDays
        : [this.exerciseForm.day];

    const request: CreateExerciseRequest = {
      exerciseTemplateId: this.exerciseForm.exerciseTemplateId,
      sets: this.exerciseForm.sets,
      reps: this.exerciseForm.reps,
      rpe: this.exerciseForm.rpe,
      day: this.exerciseForm.day, // For backward compatibility
      notes: this.exerciseForm.notes,
      days: daysToCreate, // For multiple days
    };

    this.exerciseService.createExercise(request).subscribe({
      next: (exercise) => {
        console.log('Exercise created:', exercise);
        this.modalController.dismiss(exercise, 'created');
      },
      error: (error) => {
        console.error('Error creating exercise:', error);
      },
    });
  }

  seedData() {
    this.exerciseService.seedData().subscribe({
      next: () => {
        console.log('Seed data created');
        this.loadData(); // Reload the data
      },
      error: (error) => {
        console.error('Error seeding data:', error);
      },
    });
  }

  onMuscleGroupToggle() {
    if (this.templateForm.createNewMuscleGroup) {
      this.templateForm.muscleGroupId = '';
    } else {
      this.templateForm.muscleGroupName = '';
    }
  }

  isTemplateFormValid(): boolean {
    if (!this.templateForm.name) {
      return false;
    }

    if (this.templateForm.createNewMuscleGroup) {
      return !!this.templateForm.muscleGroupName;
    } else {
      return !!this.templateForm.muscleGroupId;
    }
  }
}
