import React, { useState } from 'react';
import { TodoItemResponseModel } from '../../api/client';
import './styles.css';
import { TodoItemRow } from './TodoItemRow';
import { useFetchTodoItems, useCreateTodoItem } from '../../controller/useTodoItemApi';

interface TodoItemsPageProps {
  listName: string;
  listId: number;
}

export const TodoItemsPage: React.FC<TodoItemsPageProps> = ({ listName, listId }) => {
  const [newItemTitle, setNewItemTitle] = useState<string>('');
  const [mutationError, setMutationError] = useState<string | null>(null);

  const { data, isLoading, error, mutate } = useFetchTodoItems(listId);
  const { createTodoItem } = useCreateTodoItem(listId, mutate);

  const todoItems: TodoItemResponseModel[] = data?.items ?? [];

  const handleAddItemClick = async () => {
    if (!newItemTitle.trim()) return;
    try {
      await createTodoItem({
        title: newItemTitle
      });
      setNewItemTitle('');
      setMutationError(null);
    } catch (err: any) {
      setMutationError(err?.message || 'Failed to add item');
    }
  };

  if (isLoading) return <div>Loading items...</div>;
  if (error) {
    console.log('Error loading list item:', error);
  }

  return (
    <div className="todo-container">
      {mutationError && <div className="error-message">{mutationError}</div>}
      <h1 className="todo-list-name">{listName}</h1>
      {todoItems.map((item) => (
        <TodoItemRow
          key={item.id}
          item={item}
          listId={listId}
          mutate={mutate}
        />
      ))}
      <div className="add-item-container">
        <input
          type="text"
          value={newItemTitle}
          onChange={(e) => setNewItemTitle(e.target.value)}
          placeholder="Add new item"
          className="add-item-input"
        />
        <button className="add-item-button" onClick={handleAddItemClick}>
          +
        </button>
      </div>
    </div>
  );
};