import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-search-box',
  templateUrl: './search.component.html',
  styleUrl: './search.component.css',
  imports: [FormsModule]
})
export class SearchBoxComponent {
  @Input() placeholderText: string = 'Search...';
  @Input() value: string = '';

  @Output() textChanged = new EventEmitter<string>();

  onSearchChange(newValue: string) {
    this.textChanged.emit(newValue);
  }
}