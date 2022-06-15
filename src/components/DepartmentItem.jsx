import React from 'react';

function DepartmentItem(props) {
    function choose() {props.onClick(props.item.id);}

    return ( 
        <div className="department-item">
            <div className="name">
                {props.item.title}
            </div>
            <div className="ins">
                {props.item.instructorName}
            </div>
            <div className="choose">
                <button onClick={choose} disabled={props.disabled} style={{backgroundColor: props.disabled ? 'green' : ''}}>
                    Choose
                </button>
            </div>
        </div>
     );
}

export default DepartmentItem;