import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-top-nav',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './top-nav.component.html',
  styleUrl: './top-nav.component.css',
})
export class TopNavComponent {
  @Input() brand = 'CareAxis Hospital';
  @Input() navItems: { label: string; action: string }[] = [];
  @Input() activeAction = '';
  @Input() ctaPrimary = '';
  @Input() ctaSecondary = '';
  @Input() userName = '';
  @Input() showUserBadge = false;
  @Input() solid = false;

  @Output() itemClick = new EventEmitter<string>();
  @Output() primaryClick = new EventEmitter<void>();
  @Output() secondaryClick = new EventEmitter<void>();
}
