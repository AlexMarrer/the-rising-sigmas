import { IonicModule } from '@ionic/angular';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../header/header.component';
import { WorkoutTableComponent } from '../workout-table/workout-table.component';
import { TranslateModule } from '@ngx-translate/core';
import {
  ClerkSignInComponent,
  ClerkSignUpComponent,
  ClerkUserProfileComponent,
  ClerkUserButtonComponent,
  ClerkOrganizationProfileComponent,
  ClerkOrganizationSwitcherComponent,
  ClerkOrganizationListComponent,
  ClerkCreateOrganizationComponent,
} from '@alexmarrer/ngx-clerk';

@NgModule({
  declarations: [HeaderComponent, WorkoutTableComponent],
  imports: [
    CommonModule,
    TranslateModule,
    IonicModule,
    // Import all Clerk components
    ClerkSignInComponent,
    ClerkSignUpComponent,
    ClerkUserProfileComponent,
    ClerkUserButtonComponent,
    ClerkOrganizationProfileComponent,
    ClerkOrganizationSwitcherComponent,
    ClerkOrganizationListComponent,
    ClerkCreateOrganizationComponent,
  ],
  exports: [
    HeaderComponent,
    TranslateModule,
    WorkoutTableComponent,
    // Export all Clerk components
    ClerkSignInComponent,
    ClerkSignUpComponent,
    ClerkUserProfileComponent,
    ClerkUserButtonComponent,
    ClerkOrganizationProfileComponent,
    ClerkOrganizationSwitcherComponent,
    ClerkOrganizationListComponent,
    ClerkCreateOrganizationComponent,
  ],
})
export class SharedModule {}
