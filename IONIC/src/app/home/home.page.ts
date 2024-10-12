import { Component, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { WorkoutTableComponent } from '../workout-table/workout-table.component';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  @ViewChild(WorkoutTableComponent) workoutTableComponent: WorkoutTableComponent;

  constructor(private titleService: Title) {
    this.titleService.setTitle('Home');
  }

  ionViewWillEnter() {
    if (this.workoutTableComponent) {
      this.workoutTableComponent.changeTableView();
    }
  }
}
