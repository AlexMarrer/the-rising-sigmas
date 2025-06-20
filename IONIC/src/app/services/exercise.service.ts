import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Exercise {
  id: string;
  reps: number;
  sets: number;
  rpe: number;
  day: number;
  notes: string;
  exerciseTemplateId: string;
  weekPlanId: string;
  exerciseTemplate: ExerciseTemplate;
  weekPlan: WeekPlan;
}

export interface ExerciseTemplate {
  id: string;
  name: string;
  muscleGroupId: string;
  muscleGroup: MuscleGroup;
}

export interface MuscleGroup {
  id: string;
  name: string;
}

export interface WeekPlan {
  id: string;
  weekNumber: number;
  version: number;
  trainingPlanId: string;
}

export interface CreateExerciseRequest {
  reps: number;
  sets: number;
  rpe: number;
  day: number;
  notes: string;
  exerciseTemplateId: string;
  days?: number[];
}

export interface CreateExerciseTemplateRequest {
  name: string;
  muscleGroupId?: string;
  muscleGroupName?: string;
}

export interface CreateMuscleGroupRequest {
  name: string;
}

export interface UpdateExerciseRequest {
  reps: number;
  sets: number;
  rpe: number;
  day: number;
  notes: string;
  exerciseTemplateId: string;
}

@Injectable({
  providedIn: 'root',
})
export class ExerciseService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getAllExercises(): Observable<Exercise[]> {
    return this.http.get<Exercise[]>(`${this.baseUrl}/exercise`);
  }

  getAllExerciseTemplates(): Observable<ExerciseTemplate[]> {
    return this.http.get<ExerciseTemplate[]>(`${this.baseUrl}/exercise/templates`);
  }

  getAllMuscleGroups(): Observable<MuscleGroup[]> {
    return this.http.get<MuscleGroup[]>(`${this.baseUrl}/exercise/musclegroups`);
  }

  createExercise(request: CreateExerciseRequest): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/exercise`, request);
  }

  createExerciseTemplate(request: CreateExerciseTemplateRequest): Observable<ExerciseTemplate> {
    return this.http.post<ExerciseTemplate>(`${this.baseUrl}/exercise/templates`, request);
  }

  createMuscleGroup(request: CreateMuscleGroupRequest): Observable<MuscleGroup> {
    return this.http.post<MuscleGroup>(`${this.baseUrl}/exercise/musclegroups`, request);
  }
  updateExercise(id: string, request: UpdateExerciseRequest): Observable<Exercise> {
    return this.http.put<Exercise>(`${this.baseUrl}/exercise/${id}`, request);
  }

  seedData(): Observable<any> {
    return this.http.post(`${this.baseUrl}/exercise/seed-data`, {});
  }

  clearAllTrainingData(): Observable<any> {
    return this.http.delete(`${this.baseUrl}/exercise/clear-all-data`);
  }
}
