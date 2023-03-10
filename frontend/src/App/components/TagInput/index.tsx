import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { WithContext as ReactTags, Tag } from "react-tag-input";

import { getTags } from "../../api/endpoints/tags";

import { useAuth } from "../../hooks";

import "./index.css";

interface TagInputParams {
    tags: Tag[],
    setTags: React.Dispatch<React.SetStateAction<Tag[]>>
}

export default function TagInput({ tags, setTags }: TagInputParams) {
    const { account } = useAuth();

    const [suggestions, setSuggestions] = useState([] as Tag[]);

    const navigate = useNavigate();

    function fetchTags(value: string) {
        if (value.length < 2)
            return;

        getTags(1, value)
            .then(res => setSuggestions(res.data.items.map(t => ({ id: t.id!.toString(), text: t.name }))));
    }

    function addTag(tag: Tag) {
        if (!suggestions.some(t => t.id === tag.id))
            return;

        setTags(p => [...p, tag]);
    }

    function removeTag(item: number) {
        setTags(p => p.filter((tag, index) => index !== item));
    }

    function editTag(i: number) {
        if (!account?.admin)
            return;

        navigate(`/tag/edit/${tags[i].id}`);
    }

    function renderSuggestion(tag: Tag) {
        return <button type="button">{tag.text}</button>
    }

    return <ReactTags
        tags={tags}
        delimiters={[13]}
        autocomplete={true}
        suggestions={suggestions}
        placeholder="Enter new tag"
        allowDragDrop={false}
        handleInputChange={fetchTags}
        handleAddition={addTag}
        handleDelete={removeTag}
        handleTagClick={editTag}
        classNames={{
            tags: "tags",
            tagInput: "column tag-input",
            suggestions: "small tag-suggestions",
            remove: "tag-remove-button",
            selected: "center row selected-tags",
            tag: "tag"
        }}
        renderSuggestion={renderSuggestion}
    />
}