import { Component, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { WorkoutTableComponent } from '../workout-table/workout-table.component';

@Component({
  selector: 'app-plan',
  templateUrl: './plan.page.html',
  styleUrls: ['./plan.page.scss'],
})
export class PlanPage {
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
