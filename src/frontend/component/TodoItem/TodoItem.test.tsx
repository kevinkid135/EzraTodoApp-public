import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { TodoItemRow } from '../../component/TodoItem/TodoItemRow';
import { TodoItemsPage } from '../../component/TodoItem/TodoItemsPage';

const mockItem = {
  id: 1,
  title: 'Test Item',
  isCompleted: false,
  isDeleted: false,
  init: () => {},
  toJSON: () => ({}),
};

const mockMutate = jest.fn();

describe('Todo Item Get', () => {
  it('renders todo item title', () => {
    render(
      <TodoItemRow item={mockItem} listId={1} mutate={mockMutate} />
    );
    expect(screen.getByText('Test Item')).toBeInTheDocument();
  });
});

describe('Todo Item Create', () => {
  it('calls createTodoItem when add button is clicked', async () => {
    const createTodoItem = jest.fn().mockResolvedValue({});
    jest.spyOn(require('../../controller/useTodoItemApi'), 'useCreateTodoItem')
      .mockReturnValue({ createTodoItem });

    render(
      <TodoItemsPage listName="Test List" listId={1} />
    );
    fireEvent.change(screen.getByPlaceholderText('Add new item'), { target: { value: 'New Item' } });
    fireEvent.click(screen.getByText('+'));
    await waitFor(() => expect(createTodoItem).toHaveBeenCalledWith({ title: 'New Item' }));
  });
});

describe('Todo Item Delete', () => {
  it('calls deleteTodoItem when delete icon is clicked', async () => {
    const deleteTodoItem = jest.fn().mockResolvedValue({});
    jest.spyOn(require('../../controller/useTodoItemApi'), 'useDeleteTodoItem')
      .mockReturnValue({ deleteTodoItem });

    render(
      <TodoItemRow item={mockItem} listId={1} mutate={mockMutate} />
    );
    fireEvent.click(screen.getByText('delete'));
    await waitFor(() => expect(deleteTodoItem).toHaveBeenCalledWith(1));
  });
});

describe('Todo Item Edit', () => {
  it('allows editing and saving the todo item', async () => {
    const updateTodoItem = jest.fn().mockResolvedValue({});
    jest.spyOn(require('../../controller/useTodoItemApi'), 'useUpdateTodoItem')
      .mockReturnValue({ updateTodoItem });

    render(
      <TodoItemRow item={mockItem} listId={1} mutate={mockMutate} />
    );
    fireEvent.click(screen.getByText('edit'));
    fireEvent.change(screen.getByDisplayValue('Test Item'), { target: { value: 'Edited Item' } });
    fireEvent.click(screen.getByText('Save'));
    await waitFor(() =>
      expect(updateTodoItem).toHaveBeenCalledWith(1, expect.objectContaining({ title: 'Edited Item' }))
    );
  });
});

