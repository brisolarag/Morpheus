import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TopBar } from '../top-bar/top-bar';

@Component({
  selector: 'app-main-layout',
  imports: [RouterModule, TopBar],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css',
})
export class MainLayout { }
