import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RegisterComponent } from './register/register.component';
import { CancelComponent } from './cancel/cancel.component';
import { VerifyComponent } from './verify/verify.component';
import { VerifyAccessGuard } from './_guards/verify-access.guard';

export const routes: Routes = [
  {path:'', component: DashboardComponent},
  {path:'',
    children: [
      {path:'register', component: RegisterComponent},
      {path:'cancel', component: CancelComponent},
      { path: 'verify', component: VerifyComponent, canActivate: [VerifyAccessGuard] },
    ]
  }
];
