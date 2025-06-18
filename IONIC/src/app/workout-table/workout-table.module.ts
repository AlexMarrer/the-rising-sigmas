import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { TranslateModule } from '@ngx-translate/core';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { WorkoutTableComponent } from './workout-table.component';

@NgModule({
  declarations: [WorkoutTableComponent],
  imports: [CommonModule, DragDropModule, IonicModule, TranslateModule],
  exports: [WorkoutTableComponent],
})
export class WorkoutTableModule {}
