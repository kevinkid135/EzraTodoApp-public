import React, { useState } from 'react';
import { TodoListsPage } from '../TodoList/TodoListsPage';
import { TodoItemsPage } from '../TodoItem/TodoItemsPage';
import { TodoListResponseModel } from '../../api/client';
import './styles.css';

interface SelectedListProps {
  handleBackClick: () => void;
  selectedList: TodoListResponseModel | undefined;
}

const SelectedList: React.FC<SelectedListProps> = ({
  handleBackClick,
  selectedList,
}) => (
  <div className="todo-view-selected-list">
    <button onClick={handleBackClick} className="todo-view-back-button">
      <span className="material-icons todo-view-back-icon">arrow_back</span>
      Back
    </button>
    <TodoItemsPage
      listId={selectedList?.id ?? 0}
      listName={selectedList?.name || ''}
    />
  </div>
);

export const TodoApp: React.FC = () => {
  const [selectedList, setSelectedList] = useState<TodoListResponseModel | undefined>(undefined);

  const handleListClick = (list: TodoListResponseModel) => {
    setSelectedList(list);
  };

  const handleBackClick = () => {
    setSelectedList(undefined);
  };

  return (
    <div className="todo-view-container">
      <header className="todo-view-header">
        <span className="material-icons todo-view-icon">check_box</span>
        <h1 className="todo-view-title">Todo App</h1>
      </header>
      <main className="todo-view-main">
        {!selectedList ? (
          <TodoListsPage onListClick={handleListClick} />
        ) : (
          <SelectedList
            handleBackClick={handleBackClick}
            selectedList={selectedList}
          />
        )}
      </main>
    </div>
  );
};