import { Component } from '@angular/core';
import { ClerkService } from '@alexmarrer/ngx-clerk';
import { environment } from 'src/environments/environment';
@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  constructor(private _clerk: ClerkService) {
    this._clerk.__init({ publishableKey: environment.VITE_CLERK_PUBLISHABLE_KEY });
  }
}
