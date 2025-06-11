import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { DomSanitizer } from '@angular/platform-browser';

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.log(err));
