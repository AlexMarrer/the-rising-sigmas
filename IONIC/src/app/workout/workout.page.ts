import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-workout',
  templateUrl: './workout.page.html',
  styleUrls: ['./workout.page.scss'],
})
export class WorkoutPage implements OnInit {

  titleService?: Title;

  constructor(titleService: Title) {
    this.titleService = titleService;

  }

  ngOnInit() {
    this.titleService.setTitle('Workout');
  }

}
