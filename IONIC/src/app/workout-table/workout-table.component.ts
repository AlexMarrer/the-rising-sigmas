import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-workout-table',
  templateUrl: './workout-table.component.html',
  styleUrls: ['./workout-table.component.scss'],
})
export class WorkoutTableComponent implements OnInit {
  private button: HTMLButtonElement;
  private workoutTable: HTMLDivElement;

  ngOnInit(): void {
    this.button = document.querySelector('.workout-table__add-button');
    this.workoutTable = document.querySelector('.workout-table__table');

    this.button.addEventListener('click', () => {
      console.log('Button clicked');
      const div = document.createElement('div');
      div.innerHTML = 'Hello World';
      this.workoutTable.appendChild(div);
    });
  }
}
