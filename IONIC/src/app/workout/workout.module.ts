import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { WorkoutPageRoutingModule } from './workout-routing.module';
import { WorkoutPage } from './workout.page';

import { SharedModule } from '../shared/shared.module'; // Import SharedModule

@NgModule({
  imports: [CommonModule, FormsModule, IonicModule, WorkoutPageRoutingModule, SharedModule],
  declarations: [WorkoutPage],
})
export class WorkoutPageModule {}
