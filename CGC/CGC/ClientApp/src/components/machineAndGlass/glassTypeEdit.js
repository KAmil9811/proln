import React, { Component } from 'react';
import './glassTypeEdit.css'
import Sidebar from '../Sidebar';

export class GlassTypeEdit extends Component {
    displayName = GlassTypeEdit.name;
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
                company: sessionStorage.getItem('company'),
                login: sessionStorage.getItem('login')
            },
            new_type: this.refs.type.value,
            old_type: sessionStorage.getItem('type')
        }
        fetch(`api/Magazine/Change_type_Admin`, {
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
                    this.props.history.push('/glassatibutes')
                    sessionStorage.removeItem('typ')
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
        sessionStorage.removeItem('type')
        this.props.history.push('/glassatibutes')
    }

    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePageFail">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (

                <div className="TypeEdit">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Edit glass type</h1>
                    </div>
                    <form>
                        <div className="TypeEdit_c">

                            <div className="form-group">
                                <input
                                    type="text"
                                    name="color"
                                    className="form-control"
                                    id="inputColor"
                                    placeholder="Enter type"
                                    ref="type"
                                    defaultValue={sessionStorage.getItem('type')}
                                />
                            </div>
                            <button type="button" className="success_type_edit" onClick={this.changeColor}>Edit type</button>

                            <button type="button" className="danger_type_edit" onClick={this.return}>Cancel</button>


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