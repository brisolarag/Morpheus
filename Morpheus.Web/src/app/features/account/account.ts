import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CvService, CvModel } from '../../core/services/cv';

@Component({
  selector: 'app-account',
  imports: [CommonModule],
  templateUrl: './account.html',
  styleUrl: './account.css',
})
export class Account implements OnInit {
  private cvService = inject(CvService);
  private cdr = inject(ChangeDetectorRef);

  activeTab = 'Account';
  recentCvs: CvModel[] = [];

  tabs = [
    'Account',
    'CV',
    'Summary',
    'Professional Experience',
    'Education',
    'Skills',
    'Certificates',
    'Languages',
    'Settings'
  ];

  ngOnInit() {
    this.loadRecentCvs();
  }

  loadRecentCvs() {
    this.cvService.getRecentCvs().subscribe(cvs => {
      this.recentCvs = cvs;
      this.cdr.markForCheck();
    });
  }

  setTab(tab: string) {
    this.activeTab = tab;
  }
}
