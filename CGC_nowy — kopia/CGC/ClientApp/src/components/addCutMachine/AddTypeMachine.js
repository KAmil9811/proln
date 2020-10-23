import React, { Component } from "react";
import './AddTypeMachine.css';


export class AddTypeMachine extends Component {
    displayName = AddTypeMachine.name;
    constructor(props) {
        super(props);
    }

    handleAddType = (event) => {
        event.preventDefault();
        const receiver = {
            type: this.refs.type.value,
            user: {
                login: sessionStorage.getItem('login')
            }
        }

        fetch(`api/Machine/Add_Type_Admin`, {
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
                alert("Dodano typ maszyny do bazy danych")
                this.props.history.push('/cutmachineedit')
            })
    }

    cancelAddType = (event) => {
        this.props.history.push('/cutmachineedit')
    }

    render() {
        return (
            <div className="addTypeMachine">
                <form>
                    <div className="form-group">
                        <h2>Dodaj typ maszyny:</h2>
                        <input
                            type="text"
                            className="form-control"
                            id="inputType"
                            placeholder='Podaj typ maszyny'
                            ref="type"
                        />
                    </div>
                    <div className="form-group">
                        <button type="button" className="cancel_add_t" onClick={this.cancelAddType}>Anuluj</button>
                        <button type="button" className="add_type_mach5 " onClick={this.handleAddType}>Dodaj</button>
                        
                    </div>

                </form>
            </div>
        );
    }
}