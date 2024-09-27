import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { AccountPageRoutingModule } from './account-routing.module';
import { AccountPage } from './account.page';

import { SharedModule } from '../shared/shared.module'; // Import SharedModule

@NgModule({
  imports: [
    AccountPageRoutingModule,
    CommonModule,
    FormsModule,
    IonicModule,
    SharedModule,
  ],
  declarations: [AccountPage],
})
export class AccountPageModule {}
