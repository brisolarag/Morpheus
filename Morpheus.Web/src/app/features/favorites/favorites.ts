import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JobService } from '../../core/services/job';
import { Job } from '../../core/models/job.model';

@Component({
  selector: 'app-favorites',
  imports: [CommonModule],
  templateUrl: './favorites.html',
  styleUrl: './favorites.css',
})
export class Favorites implements OnInit {
  private jobService = inject(JobService);

  favoriteJobs: Job[] = [];
  isLoading = true;

  ngOnInit(): void {
    this.jobService.getFavorites().subscribe({
      next: (res) => {
        this.favoriteJobs = res.results;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }
}
