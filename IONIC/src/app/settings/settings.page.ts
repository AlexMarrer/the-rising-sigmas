import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { AlertController, LoadingController, ToastController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';
import { ExerciseService } from '../services/exercise.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.page.html',
  styleUrls: ['./settings.page.scss'],
})
export class SettingsPage implements OnInit {
  constructor(
    private titleService: Title,
    private router: Router,
    private exerciseService: ExerciseService,
    private alertController: AlertController,
    private loadingController: LoadingController,
    private toastController: ToastController,
    private translate: TranslateService
  ) {}

  ngOnInit() {
    this.translate.get('SETTINGS.TITLE').subscribe((title: string) => {
      this.titleService.setTitle(title);
    });
  }

  clearData() {
    localStorage.clear();
    this.router.navigate(['/']);
  }

  async clearAllTrainingData() {
    const alert = await this.alertController.create({
      header: await this.translate.get('SETTINGS.TRAINING_DATA.CONFIRM.HEADER').toPromise(),
      message: await this.translate.get('SETTINGS.TRAINING_DATA.CONFIRM.MESSAGE').toPromise(),
      buttons: [
        {
          text: await this.translate.get('SETTINGS.TRAINING_DATA.CONFIRM.CANCEL').toPromise(),
          role: 'cancel',
        },
        {
          text: await this.translate.get('SETTINGS.TRAINING_DATA.CONFIRM.DELETE').toPromise(),
          role: 'destructive',
          handler: async () => {
            await this.performClearAllData();
          },
        },
      ],
    });

    await alert.present();
  }

  private async performClearAllData() {
    const loading = await this.loadingController.create({
      message: await this.translate.get('SETTINGS.TRAINING_DATA.LOADING').toPromise(),
    });
    await loading.present();

    try {
      await this.exerciseService.clearAllTrainingData().toPromise();

      await loading.dismiss();

      const successMessage = await this.translate.get('SETTINGS.TRAINING_DATA.SUCCESS').toPromise();
      const toast = await this.toastController.create({
        message: successMessage,
        duration: 3000,
        color: 'success',
      });
      await toast.present();

      // Reload the page to update all tables and components
      window.location.reload();
    } catch (error) {
      await loading.dismiss();

      console.error('Error clearing training data:', error);

      const errorMessage = await this.translate.get('SETTINGS.TRAINING_DATA.ERROR').toPromise();
      const toast = await this.toastController.create({
        message: errorMessage,
        duration: 3000,
        color: 'danger',
      });
      await toast.present();
    }
  }
}
