import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.page.html',
  styleUrls: ['./settings.page.scss'],
})
export class SettingsPage implements OnInit {

  titleService?: Title;

  constructor(titleService: Title) {
    this.titleService = titleService;
  }

  ngOnInit() {
    this.titleService.setTitle('Settings');
  }

}
