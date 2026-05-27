import { Component, Input, ChangeDetectorRef, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CV } from '../../../core/models/cv.model';
import { Job } from '../../../core/models/job.model';

@Component({
  selector: 'app-cv-editor',
  imports: [CommonModule],
  templateUrl: './cv-editor.html',
  styleUrl: './cv-editor.css'
})
export class CvEditorComponent {
  private cdr = inject(ChangeDetectorRef);
  @Input() job: Job | null = null;
  
  selectedCv: CV | null = null;

  isAnalyzingFile = false;
  uploadProgress = 0;
  progressInterval: any;

  private baseMockContent = `GABRIEL BRISOLARA
Software Engineer | Full Stack Developer

SUMMARY
Passionate Full Stack Developer with experience in building scalable web applications. Proficient in modern JavaScript frameworks (Angular, React) and backend technologies (Node.js, .NET). Strong problem-solving skills and a team player.

EXPERIENCE
Software Engineer - Tech Solutions Inc.
Jan 2021 - Present
- Developed and maintained critical components of the core product using Angular and C#.
- Optimized database queries, reducing load times by 30%.
- Collaborated with UX designers to implement responsive, accessible interfaces.

EDUCATION
B.S. in Computer Science - University of Technology
2016 - 2020`;

  private mockSuggestions = [
    { id: 'sug-1', textRef: 'Full Stack Developer', suggestion: 'Consider emphasizing your specific experience with Cloud infrastructure (Azure/AWS) as requested in the Data Center role.' },
    { id: 'sug-2', textRef: 'Angular, React', suggestion: 'Highlight your proficiency with TypeScript and .NET specifically, as Microsoft highly values these technologies.' },
    { id: 'sug-3', textRef: 'Optimized database queries', suggestion: 'Quantify this further. Did this optimization lead to cost savings or handle a specific scale of TPS (Transactions Per Second)?' }
  ];

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.simulateAnalysis(input.files[0]);
    }
  }

  simulateAnalysis(file: File): void {
    this.isAnalyzingFile = true;
    this.uploadProgress = 0;
    this.cdr.markForCheck();
    
    // Simulate progress bar
    this.progressInterval = setInterval(() => {
      this.uploadProgress += Math.floor(Math.random() * 15) + 5;
      if (this.uploadProgress > 95) {
        this.uploadProgress = 95;
      }
      this.cdr.markForCheck();
    }, 200);

    // Simulate ~2.5 seconds of AI processing
    setTimeout(() => {
      clearInterval(this.progressInterval);
      this.uploadProgress = 100;
      this.cdr.markForCheck();
      
      setTimeout(() => {
        this.isAnalyzingFile = false;
        // Mock the extracted CV data based on the uploaded file name
        this.selectedCv = {
          id: 'cv-new-mock',
          name: file.name,
          updatedAt: new Date().toISOString(),
          content: this.baseMockContent,
          suggestions: this.mockSuggestions
        };
        this.cdr.markForCheck();
      }, 300);
    }, 2500);
  }

  selectCv(cv: CV | null): void {
    this.selectedCv = cv;
    this.isAnalyzingFile = false;
    this.uploadProgress = 0;
  }
}
