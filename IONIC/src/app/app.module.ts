import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy, RouterModule, Routes } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { HomePage } from './home/home.page';
import { WorkoutPage } from './workout/workout.page';
import { AccountPage } from './account/account.page';
import { PlanPage } from './plan/plan.page';
import { SettingsPage } from './settings/settings.page';
import { HttpClientModule, HttpClient } from '@angular/common/http';

import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

const routes: Routes = [
  { path: 'home', component: HomePage, data: { title: 'Home' } },
  { path: 'workout', component: WorkoutPage, data: { title: 'Workout' } },
  { path: 'account', component: AccountPage, data: { title: 'Account' } },
  { path: 'plan', component: PlanPage, data: { title: 'Plan' } },
  { path: 'settings', component: SettingsPage, data: { title: 'Settings' } },
  // Weitere Routen
];

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [AppComponent, HeaderComponent],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, RouterModule.forRoot(routes),
    HttpClientModule, 
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    })
  ],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
  bootstrap: [AppComponent],
})

export class AppModule {}
