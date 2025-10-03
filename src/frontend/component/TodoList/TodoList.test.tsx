import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { TodoListRow } from '../../component/TodoList/TodoListRow';
import { TodoListsPage } from '../../component/TodoList/TodoListsPage';

const mockList = {
  id: 1,
  name: 'Test List',
  init: () => {},
  toJSON: () => ({}),
};

const mockMutate = jest.fn();

describe('Todo List Get', () => {
  it('renders todo list name', () => {
    render(
      <TodoListRow list={mockList} onListClick={jest.fn()} mutate={mockMutate} />
    );
    expect(screen.getByText('Test List')).toBeInTheDocument();
  });
});

describe('Todo List Create', () => {
  it('calls createTodoList when add button is clicked', async () => {
    const createTodoList = jest.fn().mockResolvedValue({});
    jest.spyOn(require('../../controller/useTodoListApi'), 'useCreateTodoList')
      .mockReturnValue({ createTodoList });

    // Mock useFetchTodoLists to return empty data
    jest.spyOn(require('../../controller/useTodoListApi'), 'useFetchTodoLists')
      .mockReturnValue({ data: { items: [] }, isLoading: false, error: null, mutate: mockMutate });

    render(
      <TodoListsPage onListClick={jest.fn()} />
    );
    fireEvent.change(screen.getByPlaceholderText('Add new list'), { target: { value: 'New List' } });
    fireEvent.click(screen.getByText('+'));
    await waitFor(() => expect(createTodoList).toHaveBeenCalledWith({ name: 'New List' }));
  });
});

describe('Todo List Delete', () => {
  it('calls deleteTodoList when delete icon is clicked', async () => {
    const deleteTodoList = jest.fn().mockResolvedValue({});
    jest.spyOn(require('../../controller/useTodoListApi'), 'useDeleteTodoList')
      .mockReturnValue({ deleteTodoList });

    render(
      <TodoListRow list={mockList} onListClick={jest.fn()} mutate={mockMutate} />
    );
    fireEvent.click(screen.getByText('delete'));
    await waitFor(() => expect(deleteTodoList).toHaveBeenCalledWith(1));
  });
});

describe('Todo List Edit', () => {
  it('allows editing and saving the todo list', async () => {
    const updateTodoList = jest.fn().mockResolvedValue({});
    jest.spyOn(require('../../controller/useTodoListApi'), 'useUpdateTodoList')
      .mockReturnValue({ updateTodoList });

    render(
      <TodoListRow list={mockList} onListClick={jest.fn()} mutate={mockMutate} />
    );
    fireEvent.click(screen.getByText('edit'));
    fireEvent.change(screen.getByDisplayValue('Test List'), { target: { value: 'Edited List' } });
    fireEvent.click(screen.getByText('Save'));
    await waitFor(() =>
      expect(updateTodoList).toHaveBeenCalledWith(1, expect.objectContaining({ name: 'Edited List' }))
    );
  });
}
);
