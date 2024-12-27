import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { ClerkAuthGuardService } from '@alexmarrer/ngx-clerk';

const routes: Routes = [
  {
    path: '',
    canActivate: [ClerkAuthGuardService],
    loadChildren: () => import('./tabs/tabs.module').then((m) => m.TabsPageModule),
  },
  { path: '**', redirectTo: '/login' }, // Alle unbekannten Routen zur Login-Seite weiterleiten
];
@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
