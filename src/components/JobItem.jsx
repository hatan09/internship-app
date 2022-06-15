import React from 'react';

function JobItem(props) {
    function showDetail() {props.onClick(props.item);}

    return ( 
        <div className="job-item">
            <div className="title">
                {props.item.title}
            </div>
            <div className="company">
                {props.item.company}
            </div>
            <div className="slots">
                {props.item.slots}
            </div>
            <div className="detail">
                {(props.onClick == null) ? <></> : <button onClick={showDetail}> Details </button>}
            </div>
        </div>
     );
}

export default JobItem;