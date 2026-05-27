import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Job } from '../../../core/models/job.model';

@Component({
  selector: 'app-job-card',
  imports: [CommonModule],
  templateUrl: './job-card.html',
  styleUrl: './job-card.css',
})
export class JobCard {
  @Input() job!: Job;
  @Output() cardClick = new EventEmitter<Job>();
  
  getRelevancePercentage(): number {
    return Math.round(this.job.relevanceScore * 100);
  }
}
