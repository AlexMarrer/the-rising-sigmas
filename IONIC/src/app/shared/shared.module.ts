import { IonicModule } from '@ionic/angular';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../header/header.component';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [HeaderComponent],
  imports: [CommonModule, TranslateModule, IonicModule],
  exports: [HeaderComponent, TranslateModule], // Export it so it can be used in other modules
})
export class SharedModule {}
