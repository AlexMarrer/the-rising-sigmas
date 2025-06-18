import { IonicModule } from '@ionic/angular';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../header/header.component';
import { WorkoutTableModule } from '../workout-table/workout-table.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [HeaderComponent],
  imports: [CommonModule, TranslateModule, IonicModule, WorkoutTableModule],
  exports: [HeaderComponent, TranslateModule, IonicModule, WorkoutTableModule],
})
export class SharedModule {}
