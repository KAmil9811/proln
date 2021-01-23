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
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                return (json);
            })
            .then(json => {
                alert("Typ został zedytowany")
                this.props.history.push('/cutmachineedit')
                sessionStorage.removeItem('machinetype')
            })
    }

    return = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('machinetype')
        this.props.history.push('/cutmachineedit')
    }

    render() {
        return (
            <div className="editTypeM">
                <Sidebar />
                <div className="EditTypeM">
                    <form>
                        <div className="form-group">
                            <h2>Edytuj typ:</h2>
                            <input
                                type="text"
                                name="type"
                                className="form-control"
                                id="inputColor"
                                placeholder="Podaj typ"
                                ref="type"
                                defaultValue={sessionStorage.getItem('machinetype')}
                            />
                        </div>
                
                        <button type="button" className="danger_cm_edit_type" onClick={this.return}>Anuluj</button>
                        <button type="button" className="success_cm_edit_type" onClick={this.changeType}>Zatwierdź</button>
                     </form>

                </div>
            </div>
        )
    }
}