import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-workout-table',
  templateUrl: './workout-table.component.html',
  styleUrls: ['./workout-table.component.scss'],
})
export class WorkoutTableComponent implements OnInit {
  private button: HTMLButtonElement;
  private workoutTable: HTMLDivElement;
  private currentDay: HTMLDivElement;
  private currentDayIndex: number;

  ngOnInit(): void {
    // Get current day
    this.setCurrentDay();
    setInterval(() => this.setCurrentDay, 5000);

    this.button = document.querySelector('.workout-table__add-button');
    this.workoutTable = document.querySelector('.workout-table__table');

    this.button.addEventListener('click', () => {
      console.log('Button clicked');
      const div = document.createElement('div');
      div.innerHTML = 'Hello World';
      this.workoutTable.appendChild(div);
    });
  }

  setCurrentDay(): void {
    const date = new Date();
    const days = ['sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday'];
    const currentDay = days[date.getDay()];
    if (this.currentDay !== undefined && this.currentDay.dataset['day'] === currentDay) {
      return;
    }
    this.currentDayIndex = date.getDay();
    this.changeCurrentDay(currentDay);
  }

  changeView(event: any): void {
    const requestedView = event.detail.value;
    const tableHeaders = Array.from(
      document.querySelectorAll<HTMLDivElement>('.workout-table__header')
    );

    tableHeaders.forEach((header) =>
      header.parentElement.classList.remove('workout-table__column--active')
    );

    switch (requestedView) {
      case 'week':
        tableHeaders.forEach((header) =>
          header.parentElement.classList.add('workout-table__column--active')
        );
        break;
      case 'three-day':
        //Index starts from sunday = 0 and ends on saturday = 6 if its sunday we want to show friday, saturday and sunday and if its monday we want to show monday tuesday and wednesday and if its tuesday we want to schow monday tuesday and wednesday
        let startIndex = this.currentDayIndex === 0 ? 4 : this.currentDayIndex - 2;
        startIndex = this.currentDayIndex === 1 ? 0 : startIndex;
        for (let i = 0; i < 3; i++) {
          tableHeaders[startIndex + i].parentElement.classList.add('workout-table__column--active');
        }
        break;
      case 'day':
        this.currentDay.parentElement.classList.add('workout-table__column--active');
        break;
      default:
        tableHeaders.forEach((header) =>
          header.parentElement.classList.add('workout-table__column--active')
        );
        break;
    }
  }

  changeCurrentDay(currentDay: string): void {
    const tableHeaders = Array.from(
      document.querySelectorAll<HTMLDivElement>('.workout-table__header')
    );
    const lastDay = document.querySelector('.workout-table__header--current-day');

    const filteredHeader = tableHeaders.filter((header) => header.dataset['day'] == currentDay);
    filteredHeader[0].classList.add('workout-table__header--current-day');
    if (lastDay !== null && lastDay !== undefined) {
      lastDay.classList.remove('workout-table__header--current-day');
    }

    this.currentDay = filteredHeader[0];
  }
}
