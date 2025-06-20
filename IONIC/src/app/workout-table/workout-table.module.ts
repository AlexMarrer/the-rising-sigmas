import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { TranslateModule } from '@ngx-translate/core';
import { HttpClientModule } from '@angular/common/http';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { WorkoutTableComponent } from './workout-table.component';
import { AddExerciseModalComponent } from '../add-exercise-modal/add-exercise-modal.component';

@NgModule({
  declarations: [WorkoutTableComponent, AddExerciseModalComponent],
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TranslateModule,
    HttpClientModule,
    DragDropModule,
  ],
  exports: [WorkoutTableComponent],
})
export class WorkoutTableModule {}
