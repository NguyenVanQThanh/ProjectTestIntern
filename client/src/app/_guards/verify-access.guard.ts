import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class VerifyAccessGuard implements CanActivate {
  constructor(private router: Router, private toastr: ToastrService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const previousUrl = this.router.getCurrentNavigation()?.previousNavigation?.finalUrl?.toString();

    if (previousUrl === '/register' || previousUrl === '/cancel') {
      return true;
    }

    this.toastr.error("You cannot access this page directly.");
    this.router.navigate(['/']);
    return false;
  }
}
