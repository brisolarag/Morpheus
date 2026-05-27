import { Routes } from '@angular/router';
import { MainLayout } from './layout/main-layout/main-layout';
import { Search } from './features/search/search';
import { Favorites } from './features/favorites/favorites';
import { Account } from './features/account/account';
import { CvEditorPage } from './features/cv-editor-page/cv-editor-page';

export const routes: Routes = [
  {
    path: '',
    component: MainLayout,
    children: [
      { path: '', redirectTo: 'search', pathMatch: 'full' },
      { path: 'search', component: Search },
      { path: 'favorites', component: Favorites },
      { path: 'account', component: Account }
    ]
  },
  {
    path: 'cv-editor/:id',
    component: CvEditorPage
  }
];
