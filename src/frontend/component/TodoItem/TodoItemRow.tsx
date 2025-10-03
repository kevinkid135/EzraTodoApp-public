import React, { useState } from 'react';
import { TodoItemResponseModel } from '../../api/client';
import './styles.css';
import { useUpdateTodoItem, useDeleteTodoItem } from '../../controller/useTodoItemApi';

export const TodoItemRow: React.FC<{
  item: TodoItemResponseModel;
  listId: number;
  mutate: () => void;
}> = ({
  item,
  listId,
  mutate,
}) => {
  const { updateTodoItem } = useUpdateTodoItem(listId, mutate);
  const { deleteTodoItem } = useDeleteTodoItem(listId, mutate);

  const [isEditing, setIsEditing] = useState(false);
  const [editedTitle, setEditedTitle] = useState(item.title ?? '');
  const [mutationError, setMutationError] = useState<string | null>(null);

  const handleEditClick = () => {
    setIsEditing(true);
    setEditedTitle(item.title ?? '');
  };

  const handleSaveClick = async () => {
    if (!editedTitle.trim()) return;
    try {
      await updateTodoItem(item.id!, {
        title: editedTitle,
        isDeleted: item.isDeleted,
        isCompleted: item.isCompleted,
      });
      setIsEditing(false);
      setEditedTitle('');
      setMutationError(null);
    } catch (err: any) {
      setMutationError(err?.message || 'Failed to save item');
    }
  };

  const handleCancelClick = () => {
    setIsEditing(false);
    setEditedTitle(item.title ?? '');
  };

  const handleDeleteClick = async () => {
    try {
      await deleteTodoItem(item.id!);
      setMutationError(null);
    } catch (err: any) {
      setMutationError(err?.message || 'Failed to delete item');
    }
  };

  const handleCheckboxChange = async () => {
    try {
      await updateTodoItem(item.id!, {
        isCompleted: !item.isCompleted,
        title: item.title,
        isDeleted: item.isDeleted,
      });
      setMutationError(null);
    } catch (err: any) {
      setMutationError(err?.message || 'Failed to update item');
    }
  };

  return (
    <div className="todo-item">
      {mutationError && <div className="error-message">{mutationError}</div>}
      <div className="todo-item-left">
        <input
          type="checkbox"
          checked={item.isCompleted}
          onChange={handleCheckboxChange}
        />
        {isEditing ? (
          <input
            type="text"
            value={editedTitle}
            onChange={(e) => setEditedTitle(e.target.value)}
            className="todo-edit-input"
          />
        ) : (
          <span className={`todo-title ${item.isCompleted ? 'todo-title-completed' : ''}`}>
            {item.title}
          </span>
        )}
      </div>
      <div className="todo-item-right">
        {isEditing ? (
          <>
            <button className="todo-save-button" onClick={handleSaveClick}>
              Save
            </button>
            <button className="todo-cancel-button" onClick={handleCancelClick}>
              Cancel
            </button>
          </>
        ) : (
          <>
            <span className="material-icons todo-icon" onClick={handleEditClick}>
              edit
            </span>
            <span className="material-icons todo-icon" onClick={handleDeleteClick}>
              delete
            </span>
          </>
        )}
      </div>
    </div>
  );
};