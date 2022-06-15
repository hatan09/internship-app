import React from 'react';

function StudentItem(props) {
    function showDetail() {props.onClick(props.item);};

    return ( 
        <div className="student-item">
            <div className="name">
                {props.item.fullName}
            </div>
            <div className="year">
                {props.item.birthdate}
            </div>
            <div className="status">
                {props.item.isAccepted.toString()}
            </div>
            <div className="detail" onClick={showDetail}>
                {(props.onClick == null) ? <></> : <button> Details </button>}
            </div>
        </div>
     );
}

export default StudentItem;