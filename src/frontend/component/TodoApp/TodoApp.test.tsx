import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { TodoApp } from './TodoApp';

// Mock TodoListsPage and TodoItemsPage to simplify tests
jest.mock('../TodoList/TodoListsPage', () => ({
  TodoListsPage: ({ onListClick }: any) => (
    <div>
      <h1>Todo Lists</h1>
      <button onClick={() => onListClick({ id: 1, name: 'Test List', init: () => {}, toJSON: () => ({}) })}>
        Select List
      </button>
    </div>
  ),
}));
jest.mock('../TodoItem/TodoItemsPage', () => ({
  TodoItemsPage: ({ listId, listName }: any) => (
    <div>
      <h2>Items for {listName}</h2>
      <span>List ID: {listId}</span>
    </div>
  ),
}));

describe('TodoApp', () => {
  it('renders Todo Lists page by default', () => {
    render(<TodoApp />);
    expect(screen.getByText('Todo App')).toBeInTheDocument();
    expect(screen.getByText('Todo Lists')).toBeInTheDocument();
  });

  it('navigates to Todo Items page when a list is selected', () => {
    render(<TodoApp />);
    fireEvent.click(screen.getByText('Select List'));
    expect(screen.getByText('Items for Test List')).toBeInTheDocument();
    expect(screen.getByText('List ID: 1')).toBeInTheDocument();
    expect(screen.getByText('Back')).toBeInTheDocument();
  });

  it('returns to Todo Lists page when Back button is clicked', () => {
    render(<TodoApp />);
    fireEvent.click(screen.getByText('Select List'));
    fireEvent.click(screen.getByText('Back'));
    expect(screen.getByText('Todo Lists')).toBeInTheDocument();
  });
});
