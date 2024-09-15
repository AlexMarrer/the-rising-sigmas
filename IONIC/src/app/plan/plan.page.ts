import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-plan',
  templateUrl: './plan.page.html',
  styleUrls: ['./plan.page.scss'],
})
export class PlanPage implements OnInit {

  titleService: Title;

  constructor(titleService: Title) {
    this.titleService = titleService;
  }

  ngOnInit() {
    this.titleService.setTitle('Plan');
  }

}
