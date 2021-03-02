import React, { Component } from 'react';
import './editTypeMachine.css';
import Sidebar from '../Sidebar';

export class MachineTypeEdit extends Component {
    displayName = MachineTypeEdit.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
        }
    }
    changeType = (event) => {
        event.preventDefault();
        const receiver = {
            user: {
                login: sessionStorage.getItem('login')
            },
            new_type: this.refs.type.value,
            old_type: sessionStorage.getItem('machinetype')
        }
        fetch(`api/Machine/Change_Type_Admin`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
               
                return (json);
            })
            .then(json => {
                if (json[0] === 'New_Type_already_exist') {
                    alert("New type already exist")
                }
                else {
                    alert("Type has been edited")
                    this.props.history.push('/cutmachineedit')
                    sessionStorage.removeItem('machinetype')
                }
            })
    }
    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    return = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('machinetype')
        this.props.history.push('/cutmachineedit')
    }

    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                < div className = "HomePageFail" >
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login2" onClick={this.goback2} >Back to home page</button>
                </div>
            );
        }
        else if ( sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div className="EditTypeM">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Edit type machine</h1>
                    </div>
                    <form>
                        <div className="EditTypeM_c">

                            <div className="form-group">

                                <input
                                    type="text"
                                    name="type"
                                    className="form-control"
                                    id="inputColor"
                                    placeholder="Enter type"
                                    ref="type"
                                    defaultValue={sessionStorage.getItem('machinetype')}
                                />
                            </div>
                            <button type="button" className="success_cm_edit_type" onClick={this.changeType}>Edit type</button>


                            <button type="button" className="danger_cm_edit_type" onClick={this.return}>Cancel</button>




                        </div>
                    </form>
                </div>


                );
        }
        else {
            return (
                <div className="HomePageFail">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login2" onClick={this.goback2} >Back to home page</button>
                </div>
            );
        }
    }
}