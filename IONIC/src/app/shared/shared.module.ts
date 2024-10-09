import { IonicModule } from '@ionic/angular';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../header/header.component';
import { WorkoutTableComponent } from '../workout-table/workout-table.component';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [HeaderComponent, WorkoutTableComponent],
  imports: [CommonModule, TranslateModule, IonicModule],
  exports: [HeaderComponent, TranslateModule, WorkoutTableComponent], // Export it so it can be used in other modules
})
export class SharedModule {}
