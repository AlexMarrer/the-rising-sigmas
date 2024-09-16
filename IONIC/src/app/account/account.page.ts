import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-account',
  templateUrl: './account.page.html',
  styleUrls: ['./account.page.scss'],
})
export class AccountPage implements OnInit {

  private titleService: Title;

  constructor(titleService: Title) {
    this.titleService = titleService;
  }

  ngOnInit() {
    this.titleService.setTitle('Account');
  }

}
