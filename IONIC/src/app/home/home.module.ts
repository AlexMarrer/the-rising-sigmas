import { IonicModule } from '@ionic/angular';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HomePage } from './home.page';
import { HomePageRoutingModule } from './home-routing.module';

import { SharedModule } from '../shared/shared.module'; // Import SharedModule

@NgModule({
  imports: [
    HomePageRoutingModule,
    IonicModule,
    CommonModule,
    FormsModule,
    SharedModule,
  ],
  declarations: [HomePage],
})
export class HomePageModule {}
