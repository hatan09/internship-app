import React from 'react';

function ApplicantItem(props) {
    function showDetail() {props.onClick(props.item, props.jobId);};

    return ( 
        <div className="applicant-item">
            <div className="name">
                {props.item.fullName}
            </div>
            <div className="year">
                {props.item.birthdate}
            </div>
            <div className="detail" onClick={showDetail}>
                {(props.onClick == null) ? <></> : <button> Details </button>}
            </div>
        </div>
     );
}

export default ApplicantItem;