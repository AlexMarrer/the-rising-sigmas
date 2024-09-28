import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { PlanPageRoutingModule } from './plan-routing.module';
import { PlanPage } from './plan.page';

import { SharedModule } from '../shared/shared.module'; // Import SharedModule

@NgModule({
  imports: [
    PlanPageRoutingModule,
    IonicModule,
    CommonModule,
    FormsModule,
    SharedModule,
  ],
  declarations: [PlanPage],
})
export class PlanPageModule {}
