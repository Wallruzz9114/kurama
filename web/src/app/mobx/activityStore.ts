import { IActivity } from './../models/activity';
import { observable, action, computed, configure, runInAction } from 'mobx';
import { createContext, SyntheticEvent } from 'react';
import activitiesService from '../api/agent';

configure({ enforceActions: 'always' });

class ActivityStore {
  @observable activitiesRegistry = new Map<string, IActivity>();
  @observable selectedActivity: IActivity | undefined;
  @observable editMode = false;
  @observable loadingInitial = false;
  @observable submittingForm = false;
  @observable target = '';

  @computed get activitiesSortedByDate() {
    return Array.from(this.activitiesRegistry.values()).sort(
      (a, b) => Date.parse(a.date) - Date.parse(b.date)
    );
  }

  @action loadActivities = async () => {
    this.loadingInitial = true;

    try {
      const activities = await activitiesService.listAll();

      runInAction('loading activities', () => {
        activities.forEach((activity: IActivity) => {
          activity.date = activity.date.split('.')[0];
          this.activitiesRegistry.set(activity.id, activity);
        });

        this.loadingInitial = false;
      });
    } catch (error) {
      runInAction('loading activities error', () => {
        this.loadingInitial = false;
      });

      console.log(error);
    }
  };

  @action createActivity = async (activity: IActivity) => {
    this.submittingForm = true;
    try {
      await activitiesService.create(activity);

      runInAction('create activity', () => {
        this.activitiesRegistry.set(activity.id, activity);
        this.editMode = false;
        this.submittingForm = false;
      });
    } catch (error) {
      runInAction('create activity error', () => {
        this.submittingForm = false;
      });

      console.log(error);
    }
  };

  @action updateActivity = async (activity: IActivity) => {
    this.submittingForm = true;

    try {
      await activitiesService.update(activity);

      runInAction('update activity', () => {
        this.activitiesRegistry.set(activity.id, activity);
        this.selectedActivity = activity;
        this.editMode = false;
        this.submittingForm = false;
      });
    } catch (error) {
      runInAction('update activity error', () => {
        this.submittingForm = false;
      });

      console.log(error);
    }
  };

  @action deleteActivity = async (
    event: SyntheticEvent<HTMLButtonElement>,
    id: string
  ) => {
    this.submittingForm = true;
    this.target = event.currentTarget.name;

    try {
      await activitiesService.delete(id);

      runInAction('delete activity', () => {
        this.activitiesRegistry.delete(id);
        this.submittingForm = false;
        this.target = '';
      });
    } catch (error) {
      runInAction('delete activity error', () => {
        this.submittingForm = false;
        this.target = '';
      });
      console.log(error);
    }
  };

  @action openNewActivityForm = () => {
    this.editMode = true;
    this.selectedActivity = undefined;
  };

  @action openEditForm = (id: string) => {
    this.selectedActivity = this.activitiesRegistry.get(id);
    this.editMode = true;
  };

  @action cancelSelectedActivity = () => {
    this.selectedActivity = undefined;
  };

  @action cancelForm = () => {
    this.editMode = false;
  };

  @action selectActivity = (id: string) => {
    this.selectedActivity = this.activitiesRegistry.get(id);
    this.editMode = false;
  };
}

export default createContext(new ActivityStore());
