import React from 'react';

function RegisterItem(props) {
    function open() {
        props.onClick(props.item);
    }
    return ( 
        <div className="register-item">
            <div className="name">
                {props.item.fullName}
            </div>
            <div className="sid">
                {props.item.studentId}
            </div>
            <div className="year">
                {props.item.birthdate}
            </div>
            <div className="detail">
                {(props.onClick == null) ? <></> : <button onClick={open}> Details </button>}
            </div>
        </div>
     );
}

export default RegisterItem;
