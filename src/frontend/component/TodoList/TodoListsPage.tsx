import React, { useState } from 'react';
import { TodoListResponseModel } from '../../api/client';
import './styles.css';
import { TodoListRow } from './TodoListRow';
import { useFetchTodoLists, useCreateTodoList } from '../../controller/useTodoListApi';

interface TodoListsPageProps {
  onListClick: (list: TodoListResponseModel) => void;
}

export const TodoListsPage: React.FC<TodoListsPageProps> = ({ onListClick }) => {
  const [newListName, setNewListName] = useState<string>('');
  const [mutationError, setMutationError] = useState<string | null>(null);
  const { data, isLoading, error, mutate } = useFetchTodoLists();
  const { createTodoList } = useCreateTodoList(mutate);

  const todoLists: TodoListResponseModel[] = data?.items ?? [];

  const handleAddListClick = async () => {
    if (!newListName.trim()) return;
    try {
      await createTodoList({ name: newListName });
      setNewListName('');
      setMutationError(null);
    } catch (err: any) {
      setMutationError(err?.message || 'Failed to add list');
    }
  };

  if (isLoading) return <div>Loading lists...</div>;
  if (error) {
    console.log('Error loading lists:', error);
  }

  return (
    <div>
      <h1>Todo Lists</h1>
      {mutationError && <div className="error-message">{mutationError}</div>}
      {todoLists.map((list) => (
        <TodoListRow
          key={list.id}
          list={list}
          onListClick={() => onListClick(list)}
          mutate={mutate}
        />
      ))}
      {/* CreateTodoList */}
      <div className="add-list-container">
        <input
          type="text"
          value={newListName}
          onChange={(e) => setNewListName(e.target.value)}
          placeholder="Add new list"
          className="add-list-input"
        />
        <button className="add-list-button" onClick={handleAddListClick}>
          +
        </button>
      </div>
    </div>
  );
};