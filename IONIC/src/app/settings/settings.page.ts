import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.page.html',
  styleUrls: ['./settings.page.scss'],
})
export class SettingsPage implements OnInit {

  constructor(private titleService: Title, private router: Router) {}

  ngOnInit() {
    this.titleService.setTitle('Settings');
  }

  clearData() {
    localStorage.clear(); 
    this.router.navigate(['/']);
  }
}
